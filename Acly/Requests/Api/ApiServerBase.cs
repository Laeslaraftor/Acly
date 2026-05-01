using Acly.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Acly.Requests
{
    public partial class Api
    {
        /// <summary>
        /// Базовый класс сервера
        /// </summary>
        public abstract class ServerBase : Disposable
        {
            /// <summary>
			/// Создать класс для отправки запросов на сервер
			/// </summary>
			protected ServerBase()
            {
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            protected ServerBase(string baseUrl)
            {
                BaseUrl = baseUrl;
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            /// <param name="baseFileExtension">
            /// Основной тип файла. Например: php; json; html.
            /// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
            /// </param>
            protected ServerBase(string baseUrl, string baseFileExtension)
            {
                BaseUrl = baseUrl;
                BaseFileExtension = baseFileExtension;
            }
            /// <summary>
            /// Создать класс для отправки запросов на сервер
            /// </summary>
            /// <param name="baseUrl">Адрес сервера. Например, https://api.acly.ru</param>
            /// <param name="baseFileExtension">
            /// Основной тип файла. Например: php; json; html.
            /// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
            /// </param>
            /// <param name="baseParameters">
            /// Функция получения параметров, которые будут использоваться в каждом запросе.
            /// Пример выводимых параметров:
            /// <code>param=value&amp;param2=value2</code>
            /// </param>
            protected ServerBase(string baseUrl, string baseFileExtension, Func<Task<string>> baseParameters)
            {
                BaseUrl = baseUrl;
                BaseFileExtension = baseFileExtension;
                BaseParameters = baseParameters;
            }

            /// <summary>
            /// Маска запроса.
            /// Значение по умолчанию: {<see cref="BaseUrl"/>}/{Filename}{<see cref="BaseFileExtension"/>}{Parameters}.
            /// То есть: {0}/{1}{2}{3}.
            /// Здесь {Parameters} - это объединённые <see cref="BaseParameters"/> и параметры, указанные при вызове <see cref="GetUrl(string, string)"/>
            /// </summary>
            public string BaseUrlFormat
            {
                get => field ?? string.Empty;
                set
                {
                    if (field != value)
                    {
                        OnPropertyChanging(nameof(BaseUrlFormat));
                        field = value;
                        OnPropertyChanged(nameof(BaseUrlFormat));
                    }
                }
            } = "{0}/{1}{2}{3}";
            /// <summary>
            /// Основной адрес запроса. Например, https://api.acly.ru
            /// </summary>
            public string BaseUrl
            {
                get => field ?? string.Empty;
                set
                {
                    if (field != value)
                    {
                        OnPropertyChanging(nameof(BaseUrl));
                        field = value;
                        OnPropertyChanged(nameof(BaseUrl));
                    }
                }
            }
            /// <summary>
            /// Основной тип файла. Например: php; json; html.
            /// Если у файла нет типа то необходимо оставить это поле пустым или использовать <see cref="Get{T}(string, string, bool)"/> с последним аргументом false
            /// </summary>
            public string BaseFileExtension
            {
                get
                {
                    string? Result = field;

                    if (Result == null)
                    {
                        return string.Empty;
                    }
                    if (Result.Length > 0)
                    {
                        Result = '.' + Result;
                    }

                    return Result;
                }
                set
                {
                    if (field != value)
                    {
                        OnPropertyChanging(nameof(BaseFileExtension));
                        field = value;
                        OnPropertyChanged(nameof(BaseFileExtension));
                    }
                }
            }
            /// <summary>
            /// Функция получения параметров, которые будут использоваться в каждом запросе.
            /// Пример выводимых параметров:
            /// <code>param=value&amp;param2=value2</code>
            /// </summary>
            public Func<Task<string>>? BaseParameters
            {
                get => field;
                set
                {
                    if (field != value)
                    {
                        OnPropertyChanging(nameof(BaseParameters));
                        field = value;
                        OnPropertyChanged(nameof(BaseParameters));
                    }
                }
            }

            #region Запросы

            /// <summary>
            /// Сделать POST запрос и получить ответ
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера</returns>
            public abstract IAsyncTask<T> Post<T>(string filename, object? content, string? parameters, bool useBaseFileExtension)
                where T : new();
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public abstract IAsyncTask<string> PostString(string filename, object? content, string? parameters, bool useBaseFileExtension);
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            public abstract IAsyncTask PostDownload(string filename, object? content, Stream destination, string? parameters, bool useBaseFileExtension);
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="content">Содержимое для отправки на сервер</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public abstract IAsyncTask<byte[]> PostBytes(string filename, object? content, string? parameters, bool useBaseFileExtension);

            /// <summary>
            /// Сделать GET запрос и получить ответ
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера</returns>
            public abstract IAsyncTask<T> Get<T>(string filename, string? parameters, bool useBaseFileExtension)
                where T : new();
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде строки
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде строки</returns>
            public abstract IAsyncTask<string> GetString(string filename, string? parameters, bool useBaseFileExtension);
            /// <summary>
            /// Сделать GET запрос и получить ответ в виде сырых данных
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
            /// <param name="destination">Поток в который будут записаны загруженные данные</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            public abstract IAsyncTask GetDownload(string filename, Stream destination, string? parameters, bool useBaseFileExtension);
            /// <summary>
            /// Сделать POST запрос и получить ответ в виде массива байтов
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
			/// <param name="parameters">Параметры запроса</param>
			/// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Ответ сервера в виде массива байтов</returns>
            public abstract IAsyncTask<byte[]> GetBytes(string filename, string? parameters, bool useBaseFileExtension);

            #endregion

            #region Получение адреса запроса

            /// <summary>
            /// Получить адрес запроса
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <returns>Адрес запроса</returns>
            public async Task<Uri> GetUrl(string filename)
            {
                return await GetUrl(filename, string.Empty);
            }
            /// <summary>
            /// Получить адрес запроса
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Адрес запроса</returns>
            public async Task<Uri> GetUrl(string filename, bool useBaseFileExtension)
            {
                return await GetUrl(filename, string.Empty, useBaseFileExtension);
            }
            /// <summary>
            /// Получить адрес запроса
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <returns>Адрес запроса</returns>
            public async Task<Uri> GetUrl(string filename, string? parameters)
            {
                return await GetUrl(filename, parameters, true);
            }
            /// <summary>
            /// Получить адрес запроса
            /// </summary>
            /// <param name="filename">Название файла на сервере</param>
            /// <param name="parameters">Параметры запроса</param>
            /// <param name="useBaseFileExtension">Использовать указанный базовый формат файла (<see cref="BaseFileExtension"/>)</param>
            /// <returns>Адрес запроса</returns>
            /// <exception cref="ArgumentNullException">Название файла не указано</exception>
            public async Task<Uri> GetUrl(string filename, string? parameters, bool useBaseFileExtension)
            {
                if (filename == null)
                {
                    throw new ArgumentNullException(nameof(filename), "Название файла не указано");
                }

                string argParams = string.Empty;

                if (parameters != null)
                {
                    argParams = parameters;
                }

                string baseParameters = await GetBaseParametersValue();
                string combinedParameters = Ajax.CombineUrlParameters(baseParameters, argParams);
                string extension = string.Empty;

                if (useBaseFileExtension)
                {
                    extension = BaseFileExtension;
                }

                var formattedUrl = string.Format(BaseUrlFormat, BaseUrl, filename, extension, combinedParameters);

                return new(formattedUrl, UriKind.Absolute);
            }

            private async Task<string> GetBaseParametersValue()
            {
                if (BaseParameters != null)
                {
                    try
                    {
                        return await BaseParameters.Invoke();
                    }
                    catch (Exception error)
                    {
                        Log.Error(error);
                    }
                }

                return string.Empty;
            }

            #endregion
        }
    }
}
