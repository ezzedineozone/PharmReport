using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace PharmReportWinui.Models
{
	public class ConfileManager : IDisposable
	{
		private readonly object _sync = new object();
		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
		private FileSystemWatcher? _watcher;
		private bool _disposed;

		public string FilePath { get; }

		public Dictionary<string, JsonElement> Config { get; private set; } = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

		public DateTime? LastLoaded { get; private set; }

		public ConfileManager()
			: this(Path.Combine(AppContext.BaseDirectory, "confile.json"))
		{
		}

		public ConfileManager(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be empty", nameof(path));
			FilePath = Path.GetFullPath(path);
			EnsureDirectoryExists(FilePath);
			Load();
		}

		private static void EnsureDirectoryExists(string filePath)
		{
			var dir = Path.GetDirectoryName(filePath);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
		}

		public void Load()
		{
			lock (_sync)
			{
				if (!File.Exists(FilePath))
				{
					File.WriteAllText(FilePath, "{}", System.Text.Encoding.UTF8);
					Config = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
					LastLoaded = DateTime.UtcNow;
					return;
				}

				var json = File.ReadAllText(FilePath);
				try
				{
					var doc = JsonDocument.Parse(json);
					var root = doc.RootElement;
					var dict = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
					if (root.ValueKind == JsonValueKind.Object)
					{
						foreach (var prop in root.EnumerateObject()) dict[prop.Name] = prop.Value.Clone();
					}
					Config = dict;
					LastLoaded = DateTime.UtcNow;
				}
				catch (JsonException)
				{
					throw;
				}
			}
		}

		public void Save()
		{
			lock (_sync)
			{
				using var stream = File.Create(FilePath);
				using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
				writer.WriteStartObject();
				foreach (var kv in Config)
				{
					kv.Value.WriteTo(writer);
				}
				stream.SetLength(0);
				JsonSerializer.Serialize(stream, ToObjectMap(), _jsonOptions);
			}
		}

		private Dictionary<string, object?> ToObjectMap()
		{
			var map = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
			foreach (var kv in Config)
			{
				map[kv.Key] = JsonElementToObject(kv.Value);
			}
			return map;
		}

		private static object? JsonElementToObject(JsonElement element)
		{
			switch (element.ValueKind)
			{
				case JsonValueKind.String: return element.GetString();
				case JsonValueKind.Number:
					if (element.TryGetInt64(out var l)) return l;
					if (element.TryGetDouble(out var d)) return d;
					return element.GetRawText();
				case JsonValueKind.True: return true;
				case JsonValueKind.False: return false;
				case JsonValueKind.Object:
					var obj = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
					foreach (var p in element.EnumerateObject()) obj[p.Name] = JsonElementToObject(p.Value);
					return obj;
				case JsonValueKind.Array:
					var list = new List<object?>();
					foreach (var it in element.EnumerateArray()) list.Add(JsonElementToObject(it));
					return list;
				case JsonValueKind.Null: return null;
				default: return element.GetRawText();
			}
		}

		public T? Get<T>(string key, T? defaultValue = default)
		{
			if (key is null) throw new ArgumentNullException(nameof(key));
			lock (_sync)
			{
				if (!Config.TryGetValue(key, out var el)) return defaultValue;
				try
				{
					var raw = el.GetRawText();
					return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public void Set<T>(string key, T value)
		{
			if (key is null) throw new ArgumentNullException(nameof(key));
			lock (_sync)
			{
				var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);
				using var doc = JsonDocument.Parse(bytes);
				Config[key] = doc.RootElement.Clone();
			}
		}

		public bool Remove(string key)
		{
			if (key is null) throw new ArgumentNullException(nameof(key));
			lock (_sync)
			{
				return Config.Remove(key);
			}
		}

		public bool ContainsKey(string key)
		{
			if (key is null) throw new ArgumentNullException(nameof(key));
			lock (_sync) return Config.ContainsKey(key);
		}

		public void StartWatching(Action? onChanged = null)
		{
			lock (_sync)
			{
				if (_watcher != null) return;
				var dir = Path.GetDirectoryName(FilePath) ?? AppContext.BaseDirectory;
				_watcher = new FileSystemWatcher(dir)
				{
					Filter = Path.GetFileName(FilePath),
					NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
				};
				FileSystemEventHandler handler = (s, e) =>
				{
					Thread.Sleep(50);
					try
					{
						Load();
						onChanged?.Invoke();
					}
					catch
					{
					}
				};
				_watcher.Changed += handler;
				_watcher.Created += handler;
				_watcher.Renamed += (s, e) => handler(s, e);
				_watcher.EnableRaisingEvents = true;
			}
		}

		public void StopWatching()
		{
			lock (_sync)
			{
				if (_watcher == null) return;
				_watcher.EnableRaisingEvents = false;
				_watcher.Dispose();
				_watcher = null;
			}
		}

		public void Dispose()
		{
			if (_disposed) return;
			StopWatching();
			_disposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
