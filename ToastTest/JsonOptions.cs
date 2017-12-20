using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToastTest {
    class JsonOptions {
        public static JArray options = new JArray();
        public class Option {
            public string option { get; set; }
            public string result { get; set; }
        };
        public static void DeleteAll() {
            File.Delete("./options.json");
            options.Clear();
        }
        public static int GetOptionsCount() {
            return options.Count;
        }
        public static bool GetAllOptions() {
            if(File.Exists("./options.json")) {
                options.Clear();
                JsonSerializer serializer = new JsonSerializer();
                using(FileStream fs = File.Open("./options.json", FileMode.Open))
                using(StreamReader sr = new StreamReader(fs))
                using(JsonReader reader = new JsonTextReader(sr)) {
                    while(reader.Read() == true) {
                        if(reader.TokenType == JsonToken.StartObject) {
                            Option _option = serializer.Deserialize<Option>(reader);
                            JObject option = new JObject(
                                new JProperty("option", _option.option),
                                new JProperty("result", _option.result)
                            );
                            options.Add(option);
                        }
                    }
                    reader.Close();
                    sr.Close();
                    sr.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                return true;
            }
            return false;
        }
        public static void UpdateOptions() {
            string json = JsonConvert.SerializeObject(options, Formatting.Indented);
            File.WriteAllText("./options.json", json);
        }
        public static bool SetOption(string option, string result) {
            JObject obj = new JObject(
                new JProperty("option", option),
                new JProperty("result", result)
            );
            options.Add(obj);
            return false;
        }
        public static string GetOption(string optionString) {
            for(int i = 0; i < options.Count; i++) {
                JObject option = (JObject)options[i];
                if(((string)option.GetValue("option")) == optionString) {
                    string value = ((string)option.GetValue("result"));
                    return value;
                }
            }
            return "";
        }
    }
}
