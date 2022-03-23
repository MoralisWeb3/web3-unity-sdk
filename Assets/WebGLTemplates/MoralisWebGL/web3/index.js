// Load web3modal to connect to wallet
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3modal.js" }));
// Load web3js to create transactions
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3.min.js" }));

window.web3ChainId = 1;

// Define web3gl to unity interface
window.web3gl = {
  networkId: 0,
  debugMode: false,
  connect,
  connectAccount: "",
  signMessage,
  signMessageResponse: "",
  sendTransaction,
  sendTransactionResponse: "",
  sendContract,
  sendContractResponse: "",
};

window.moralisLiveQueries = {
    requestId = 0,
    webSockets = {},
    openSocket,
    openSocketResponse="",
    closeSocket,
    closeSocketResponse="",
    sendRequest,
    getErrors,
    getMessages,
    getSocketState
};

let provider;
let web3;

async function openSocket(key, path) {
    
    let reqId = window.moralisLiveQueries.requestId++;
    let ws = {
        requestId = reqId,
        socket = new WebSocket(path),
        messages = [],
        errors = [],
        onmessage = function (e) {
            messages.push(e.Data);
        },
        onerror = function (e) {
            errora.push(e.message);
        }
    };

    ws.socket.onopen = function (e) {
        window.moralisLiveQueries.openSocketResponse = '${key} connection established.';
    };

    ws.socket.onclose = function (event) {
        if (event.wasClean) {
            window.moralisLiveQueries.closeSocketResponse = '[${key} close] Connection closed cleanly, code=${event.code} reason=${event.reason}';
        } else {
            // e.g. server process killed or network down
            // event.code is usually 1006 in this case
            window.moralisLiveQueries.closeSocketResponse = '[${key} close] Connection died';
        }
    };

    ws.socket.onmessage = ws.onmessage;
    ws.socket.onerror = ws.onerror;

    window.moralisLiveQueries.webSockets[key] = ws;
};

function getSocketState(key) {
    var state = 3; // default to closed.
    if (window.moralisLiveQueries.webSockets[key]) {
        state = window.moralisLiveQueries.webSockets[key].readyState();
    }
    return state;
};

function closeSocket(key) {
    if (window.moralisLiveQueries.webSockets[key]) {
        window.moralisLiveQueries.webSockets[key].close();
    }
};

function sendRequest(key, msg) {
    if (window.moralisLiveQueries.webSockets[key]) {
        window.moralisLiveQueries.webSockets[key].send(msg);
    }
};

// Get any messages in the message queue of websoket key.
function getMessages(key) {
    var resp = [];

    if (window.moralisLiveQueries.webSockets[key]) {
        resp = [...window.moralisLiveQueries.webSockets[key].messages];
        window.moralisLiveQueries.webSockets[key].messages = [];
    }

    return resp;
};

// Get any errors in the error queue of websoket key.
function getErrors(key) {
    var resp = [];

    if (window.moralisLiveQueries.webSockets[key]) {
        resp = [...window.moralisLiveQueries.webSockets[key].errors];
        window.moralisLiveQueries.webSockets[key].errors = [];
    }

    return resp;
};


/*
Establish connection to web3.
*/
async function connect(appLogo, appTitle, appDesc) {
  
  const providerOptions = {
	injected: {
		display: {
		  logo: appLogo,
		  name: appTitle,
		  description: appDesc
		},
		package: null
	}
  };

  const web3Modal = new window.Web3Modal.default({
    providerOptions,
  });

  web3Modal.clearCachedProvider();

  // Create a provider
  provider = await web3Modal.connect();
  // Instantiate Web3
  web3 = new Web3(provider);

  // Set network id from the connected provider.
  web3gl.networkId = parseInt(provider.chainId);

  // Set system chain id
  if (web3gl.networkId != window.web3ChainId) {
	window.web3ChainId = web3gl.networkId;
  }

  // Set the current account from the provider.
  web3gl.connectAccount = provider.selectedAddress;

  // If the account has changed, reload the page.
  provider.on("accountsChanged", (accounts) => {
    window.location.reload();
  });

  // Update chain id if player changes network.
  provider.on("chainChanged", (chainId) => {
    web3gl.networkId = parseInt(chainId);
  });
}

/*
Implement sign message
*/
async function signMessage(message) {
  try {
    const from = (await web3.eth.getAccounts())[0];
	
	log('signMessage: message: ' + message);
	
    const signature = await web3.eth.personal.sign(message, from, "");
    window.web3gl.signMessageResponse = signature;
	  log('signMessage: signature: ' + signature);
  } catch (error) {
    window.web3gl.signMessageResponse = error.message;
	  log('signMessage: error: ' + error.message);
  }
}

/*
Implement send transaction
*/
async function sendTransaction(to, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
  
  log('sendTransaction to: ' + to + ' value: ' + value + ' gasLimit: ' + gasLimit + ' gasPrice: ' + gasPrice);
  
  web3.eth
    .sendTransaction({
      from,
      to,
      value,
      gas: gasLimit ? gasLimit : undefined,
      gasPrice: gasPrice ? gasPrice : undefined,
    })
    .on("transactionHash", (transactionHash) => {
      window.web3gl.sendTransactionResponse = transactionHash;
	  log('sendTransaction: txnHash: ' + transactionHash);
    })
    .on("error", (error) => {
      window.web3gl.sendTransactionResponse = error.message;
	  log('sendTransaction: error: ' + error.message);
    });
}

/*
Implement send contract
*/
async function sendContract(method, abi, contract, args, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
  
  log('sendContract method: ' + method + ' value: ' + value + ' gasLimit: ' + gasLimit + ' gasPrice: ' + gasPrice + ' args: ' + args);
  
  new web3.eth.Contract(JSON.parse(abi), contract).methods[method](...JSON.parse(args))
    .send({
      from,
      value,
      gas: gasLimit ? gasLimit : undefined,
      gasPrice: gasPrice ? gasPrice : undefined,
    })
    .on("transactionHash", (transactionHash) => {
      window.web3gl.sendContractResponse = transactionHash;
	  log('sendContract: txnHash: ' + transactionHash);
    })
    .on("error", (error) => {
      window.web3gl.sendContractResponse = error.message;
	  log('sendContract: error: ' + error.message);
    });
}

function log(msg){
	if (window.web3gl.debugMode) {
		console.log("web3gl: " + msg);
	}
}


