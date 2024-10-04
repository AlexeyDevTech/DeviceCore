using ANG24.Core.Devices.Base.Abstract;
using ANG24.Core.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base.Behaviors.CommandBehavior
{
    public class OrderStrongCommandDeviceBehavior : CommandDeviceBehaviorBase
    {
        public bool AutoResponse { get; set; } = false;
        public int MessageAttempts { get; set; } = 3;

        public override void HandleData(object data)
        {
            //check...
            Check(data);
        }

        public override void RequestData()
        {
            //[not required] request...
        }

        protected override void CommandTick()
        {
            //set command...
        }

        private void Check(object msg)
        {
            var result = Command.Execute(msg);
            if (!Command.Redirected) //если команда простая
            {
                if (result) OnSuccess();
                else
                {
                    if (AutoResponse)
                    {
                        if (MessageAttempts == 0)
                            OnFailure();
                        MessageAttempts--;
                    }
                    else
                    {
                        OnFailure();
                    }
                }
            }
            else
            {
                var state = Command.GetState();
                switch (state)
                {
                    case OptionalBehaviorState.Success:
                        OnSuccess();
                        Console.Out.WriteLine("[behavior success]");
                        break; //якорь завершения операции
                    case OptionalBehaviorState.Processing:
                        OnProcessing();
                        Console.Out.WriteLine("[behavior processing]");
                        break; //продолжение операции и задержка главного цикла
                    case OptionalBehaviorState.Fail:
                        OnFailure();
                        Console.Out.WriteLine("[behavior failure]");
                        break; //Якорь завершения операции с ошибкой (если не исчерпались попытки -- повторение)
                }
            }
        }

    }
}
