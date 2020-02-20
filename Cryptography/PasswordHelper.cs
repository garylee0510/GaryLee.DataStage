using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace GaryLee.DataStage.Cryptography {
	public static class PasswordHelper {
		private static Dictionary<string, string[]> dict = null;

		static PasswordHelper() {
			dict = new Dictionary<string, string[]>();
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GaryLee.DataStage.Cryptography.PasswordDictionary.txt")) {
				using (StreamReader reader = new StreamReader(stream)) {
					while (reader.Peek() > 0) {
						string key = "";
						List<string> values = new List<string>();
						foreach (string cell in reader.ReadLine().Split('\t')) {
							if (key == "") {
								key = cell;
							} else {
								values.Add(cell);
							}
						}
						dict.Add(key, values.ToArray());
					}
				}
			}
		}

		public static string Encrypt(string s) {
			StringBuilder sb = new StringBuilder();
			int len = 1;
			for (int i = 0; i < s.Length / len; i++) {
				string lookup = s.Substring(i * len, len);
				foreach (KeyValuePair<string, string[]> kv in dict) {
					if (lookup == kv.Key) {
						sb.Append(kv.Value[i % kv.Value.Length]);
						break;
					}
				}
			}
			return sb.ToString();
		}

		public static string Decrypt(string s) {
			StringBuilder sb = new StringBuilder();
			int len = 3;
			for (int i = 0; i < s.Length / len; i++) {
				string lookup = s.Substring(i * len, len);
				foreach (KeyValuePair<string, string[]> kv in dict) {
					if (lookup == kv.Value[i % kv.Value.Length]) {
						sb.Append(kv.Key);
						break;
					}
				}
			}
			return sb.ToString();
		}
	}
}
