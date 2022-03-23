
mergeInto(LibraryManager.library, {
  MoralisLiveQueries: function () {
  },
  
  OpenWebsocketJs: function (key, path) {
    window.moralisLiveQueries.openSocket(Pointer_stringify(key), Pointer_stringify(path));
  },
  
  OpenWebsocketResponse: function () {
    var bufferSize = lengthBytesUTF8(window.moralisLiveQueries.openSocketResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.moralisLiveQueries.openSocketResponse, buffer, bufferSize);
    return buffer; 
  },
  
  CloseWebsocketJs: function (key) {
    window.moralisLiveQueries.closeSocket(key);
  },
  
  CloseWebsocketResponse: function () {
    var bufferSize = lengthBytesUTF8(window.moralisLiveQueries.closeSocketResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.moralisLiveQueries.closeSocketResponse, buffer, bufferSize);
    return buffer; 
  },
  
  SendMessageJs: function (key, message) {
    window.moralisLiveQueries.sendRequest(Pointer_stringify(key), Pointer_stringify(message));
  },

  GetErrorQueueJs: function (key) {
    var errors = window.moralisLiveQueries.getErrors(Pointer_stringify(key));
    var errorString = Pointer_stringify(errors);
    var bufferSize = lengthBytesUTF8(errorString) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(errorString, buffer, bufferSize);
    return buffer; 
  },

  GetResponseQueueJs: function (key) {
    var errors = window.moralisLiveQueries.getMessages(Pointer_stringify(key));
    var respString = Pointer_stringify(getMessages);
    var bufferSize = lengthBytesUTF8(respString) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(respString, buffer, bufferSize);
    return buffer; 
  },

  GetSocketStateJs: function (key) {
    return window.moralisLiveQueries.getSocketState(Pointer_stringify(key));
  }
});
