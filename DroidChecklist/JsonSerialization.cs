using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Security;

namespace DroidChecklist
{
    public class DataOperation<T>
    {
        public List<T> Data { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool Success { get; set; }

        /// <summary>
        /// Default constructor of data operation and assumes the result was successful. Also creates a new list of specified generic type.
        /// </summary>
        public DataOperation()
        {
            Data = new List<T>();
            Message = "Successful operation";
            Success = true;
        }
    }

    /// <summary>
    /// Handles JSON serialization using the Newtonsoft JSON.NET library
    /// </summary>
    /// <typeparam name="T">Type of data that will be used</typeparam>
    public static class JsonSerialization<T>
    {
        /// <summary>
        /// Serializes a list of data in JSON format
        /// </summary>
        /// <param name="path">File to serialize to</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>Data operation</returns>
        public static DataOperation<T> SerializeList(string path, List<T> data)
        {
            DataOperation<T> dataOperation = new DataOperation<T>();
            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, data);
                }
            }
            catch(Exception ex)
            {
                dataOperation.Success = false;
                dataOperation.Exception = ex;
                dataOperation.Message = "Error occured while saving";
            }
            return dataOperation;
        }

        /// <summary>
        /// Deserializes a JSON format file into a list
        /// </summary>
        /// <param name="path">File to deserialize on</param>
        /// <returns>Data operation</returns>
        public static DataOperation<T> DeserializeList(string path)
        {
            DataOperation<T> dataOperation = new DataOperation<T>();
            try
            {

                using (StreamReader sr = File.OpenText(path))
                {
                    var text = sr.ReadToEnd();
                    dataOperation.Data = JsonConvert.DeserializeObject<List<T>>(text);
                }
            }
            catch(Exception ex)
            {
                dataOperation.Success = false;
                dataOperation.Exception = ex;
                dataOperation.Message = "Error occured while reading";
            }
            return dataOperation;
        }
    }
}
