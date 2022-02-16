// Load web3modal to connect to wallet
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3modal.js" }));
// Load web3js to create transactions
document.body.appendChild(Object.assign(document.createElement("script"), { type: "text/javascript", src: "./web3/lib/web3.min.js" }));

window.web3ChainId = 1;

// Define web3gl to unity interface
window.web3gl = {
  networkId: 0,
  connect,
  connectAccount: "",
  signMessage,
  signMessageResponse: "",
  sendTransaction,
  sendTransactionResponse: "",
  sendContract,
  sendContractResponse: "",
};

let provider;
let web3;

/*
Establish connection to web3.
*/
async function connect() {
  // Options that can be used to connect to non-standard wallets 
  // and wallet conect.
  const providerOptions = {
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
    const signature = await web3.eth.personal.sign(message, from, "");
    window.web3gl.signMessageResponse = signature;
  } catch (error) {
    window.web3gl.signMessageResponse = error.message;
  }
}

/*
Implement send transaction
*/
async function sendTransaction(to, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
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
    })
    .on("error", (error) => {
      window.web3gl.sendTransactionResponse = error.message;
    });
}

/*
Implement send contract
*/
async function sendContract(method, abi, contract, args, value, gasLimit, gasPrice) {
  const from = (await web3.eth.getAccounts())[0];
  new web3.eth.Contract(JSON.parse(abi), contract).methods[method](...JSON.parse(args))
    .send({
      from,
      value,
      gas: gasLimit ? gasLimit : undefined,
      gasPrice: gasPrice ? gasPrice : undefined,
    })
    .on("transactionHash", (transactionHash) => {
      window.web3gl.sendContractResponse = transactionHash;
    })
    .on("error", (error) => {
      window.web3gl.sendContractResponse = error.message;
    });
}
