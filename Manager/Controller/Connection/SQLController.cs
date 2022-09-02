using System;
using System.IO;
using System.Text.Json;
using Work_with_SQL_Table__HW_4_;

namespace Dapper_BDSQL.Controller
{
    class SQLController
    {
        readonly string fileName;
        public DBSettings defaultsetting { get; set; }
        public SQLController(string fileName = "setting.json")
        {
            this.fileName = fileName;
        }
        //Загрузка данных о параметрах подключения к БД из файла в формате JSON и их хранение в члене класса формата DBSettings
        public void Download()
        {
            MyLog.Log($"Start reading defaultsetting from file [{fileName}]", LogLevel.Information);
            try
            {
                defaultsetting = JsonSerializer.Deserialize<DBSettings>(File.ReadAllText(fileName));
            }
            catch (Exception ex)
            {
                MyLog.Log(ex.Message);
            }
            MyLog.Log($"Defaultsetting from file [{fileName}] was read successfully", LogLevel.Information);
        }
        public void SetSettings(DBSettings setting)
        {
            defaultsetting = setting;
        }
        //Сохранение параметров подключения к БД в файле в формате JSON
        public void Save() => File.WriteAllText(fileName, JsonSerializer.Serialize<DBSettings>(defaultsetting));
    }
}
