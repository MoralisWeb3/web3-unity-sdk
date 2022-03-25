
mergeInto(LibraryManager.library, {
  MoralisLiveQueries: function () {
  },
  
  OpenWebsocketJs: function (key, path) {
    window.moralisLiveQueries.openSocket(Pointer_stringify(key), Pointer_stringify(path));
  },
  
  OpenWebsocketResponse: function () {
    console.log("mlqgl ... OpenWebsocketResponse called: " + window.moralisLiveQueries.openSocketResponse);
    
    var bufferSize = lengthBytesUTF8(window.moralisLiveQueries.openSocketResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.moralisLiveQueries.openSocketResponse, buffer, bufferSize);
    return buffer; 
    /*
    var result = "";

    if(window.moralisLiveQueries.openSocketResponse) {
        //var resp = JSON.parse(window.moralisLiveQueries.openSocketResponse);
        result = window.moralisLiveQueries.openSocketResponse;
    }

    console.log("mlqgl ... OpenWebsocketResponse returning: " + result);

    return result;
    */
  },
  
  CloseWebsocketJs: function (key) {
    console.log("mlqgl ... CloseWebsocketJs called.");
    window.moralisLiveQueries.closeSocket(Pointer_stringify(key));
  },
  
  CloseWebsocketResponse: function () {
    console.log("mlqgl ... CloseWebsocketResponse called.");
    var bufferSize = lengthBytesUTF8(window.moralisLiveQueries.closeSocketResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.moralisLiveQueries.closeSocketResponse, buffer, bufferSize);
    return buffer; 
  },
  
  SendMessageJs: function (key, message) {
    console.log("mlqgl ... SendMessageJs called.");
    window.moralisLiveQueries.sendRequest(Pointer_stringify(key), Pointer_stringify(message));
  },

  GetErrorQueueJs: function (key) {
    console.log("mlqgl ... GetErrorQueueJs called.");
    var errors = window.moralisLiveQueries.getErrors(Pointer_stringify(key));
    var errorString = Pointer_stringify(errors);
    var bufferSize = lengthBytesUTF8(errorString) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(errorString, buffer, bufferSize);
    return buffer; 
  },

  GetResponseQueueJs: function (key) {
    console.log("mlqgl ... GetResponseQueueJs called.");
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
