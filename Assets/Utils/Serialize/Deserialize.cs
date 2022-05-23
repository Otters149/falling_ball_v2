using System;

namespace utilpackages
{
    namespace serialize
    {
        public class Deserialize
        {
            private Executor _executor;
            private qlog.QLog _logger;
            public Deserialize()
            {
                _executor = Executor.GetInstance();
                _logger = qlog.QLog.GetInstance();
            }

            public string Execute(string data)
            {
                if (_executor == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "Executor was null for de-serialize data");
                    return String.Empty;
                }

                return _executor.Execute(ActionExecute.DESERIALIZE, data);
            }
        }
    }
}
