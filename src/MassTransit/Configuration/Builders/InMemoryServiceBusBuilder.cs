// Copyright 2007-2014 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Builders
{
    using Pipeline;
    using Transports;


    public class InMemoryServiceBusBuilder :
        ServiceBusBuilderBase,
        IInMemoryServiceBusBuilder
    {
        readonly IReceiveTransportProvider _receiveTransportProvider;
        readonly ISendTransportProvider _sendTransportProvider;

        public InMemoryServiceBusBuilder(IReceiveTransportProvider receiveTransportProvider, ISendTransportProvider sendTransportProvider)
        {
            _receiveTransportProvider = receiveTransportProvider;
            _sendTransportProvider = sendTransportProvider;
        }

        public ISendTransportProvider SendTransportProvider
        {
            get { return _sendTransportProvider; }
        }

        public IReceiveTransportProvider ReceiveTransportProvider
        {
            get { return _receiveTransportProvider; }
        }

        public virtual IBusControl Build()
        {
            IInboundPipe inboundPipe = new InboundPipe();

            ISendEndpointProvider sendEndpointProvider = CreateSendEndpointProvider();

            return new SuperDuperServiceBus(inboundPipe, sendEndpointProvider, ReceiveEndpoints);
        }

        ISendEndpointProvider CreateSendEndpointProvider()
        {
            var provider = new SendEndpointProvider(_sendTransportProvider, MessageSerializer);

            return new SendEndpointCache(provider);
        }
    }
}