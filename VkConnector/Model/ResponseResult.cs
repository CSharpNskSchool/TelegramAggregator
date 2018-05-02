namespace VkConnector.Model
{
    /// <summary>
    ///     Ответ на запрос
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        ///     Результат обработки запроса
        /// </summary>
        public bool IsOk { get; set; }

        /// <summary>
        ///     Краткое описание результата
        /// </summary>
        public string Description { get; set; }
    }
}