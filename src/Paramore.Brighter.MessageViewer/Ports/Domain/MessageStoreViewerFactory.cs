// ***********************************************************************
// Assembly         : paramore.brighter.commandprocessor
// Author           : ian
// Created          : 25-03-2014
//
// Last Modified By : ian
// Last Modified On : 25-03-2014
// ***********************************************************************
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#region Licence
/* The MIT License (MIT)
Copyright � 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the �Software�), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System.Linq;
using Paramore.Brighter.MessageViewer.Adaptors.API.Configuration;
using Paramore.Brighter.MessageViewer.Ports.Domain.Config;

namespace Paramore.Brighter.MessageViewer.Ports.Domain
{
    public interface IMessageStoreViewerFactory
    {
        IAmAMessageStore<Message> Connect(string messageStoreName);
    }

    public class MessageStoreViewerFactory : IMessageStoreViewerFactory
    {
        private readonly IMessageStoreConfigProvider _messageStoreConfigProvider;
        private readonly IMessageStoreListCacheLoader _messageStoreListCache;
        private IMessageStoreConfigCache _messageStoreList;

        public MessageStoreViewerFactory(IMessageStoreConfigProvider messageStoreConfigProvider, 
            IMessageStoreListCacheLoader messageStoreListCache)
        {
            _messageStoreConfigProvider = messageStoreConfigProvider;
            _messageStoreListCache = messageStoreListCache;
        }

        public IAmAMessageStore<Message> Connect(string messageStoreName)
        {
            if (_messageStoreList == null)
            {
                _messageStoreList = _messageStoreListCache.Load();
            }
            var stores = _messageStoreConfigProvider.Get();
            MessageStoreConfig foundStore = stores.Single(s => s.Name == messageStoreName);

            return _messageStoreList.Get(foundStore);
        }
    }
}