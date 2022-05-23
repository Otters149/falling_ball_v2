using System;

namespace utilpackages
{
    namespace serialize
    {
        public class Serialize
        {
            private Executor _executor;
            private qlog.QLog _logger;
            public Serialize()
            {
                _executor = Executor.GetInstance();
                _logger = qlog.QLog.GetInstance();
            }

            public string Excute(string data)
            {
                if(_executor == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "Executor was null for serialize data");
                    return String.Empty;
                }

                return _executor.Execute(ActionExecute.SERIALIZE, data);
            }
        }
    }
}
