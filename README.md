# MyDashWallet
https://MyDashWallet.org provides an easy way for anyone to create VP wallets locally in the browser or use existing hardware wallets and send VP to anyone. For details about VP see www.dash.org

This project is open-source, free to use and development is funded by the VP DAO (Decentralized Autonomous Organization): https://www.dashcentral.org/p/MyDashWallet

# Runs locally
You can simply download the website https://MyDashWallet.org and run it locally, the whole wallet functionality (including hardware wallets, creating keystore wallets, unlocking, etc.) all runs offline and in your browser. However when you want to see the current dash amount or send out dash you need to be online as the website communicates with https://www.vpubchain.net/abe/chain/Dash for the dash amount in your addresses and will communicate with the full Dash node on MyDashWallet when sending out locally signed transactions.

If you have your own full node you can simply replace it by opening up the source code and either change the connection parameters in TestnetDashNode.cs or simply derive from that class and enter your own rpc username, password and IP (and use it in Startup.cs for your own hosted or local website). Please note that all signing happens locally in your browser, this will not increase the security of your transaction in any way, but if it makes you happy not to rely on third parties for sending signed transactions into the Dash network, feel free to run your own. Obviously anyone can simply copy the website and host it on their own server, we cannot gurantee that such sites don't change how the service works, thus you should ONLY trust this url https://MyDashWallet.org and we can only help with server errors if you actually use this site. In any case, the risk of anything malicous happen is very low, especially if you use a hardware wallet and ALWAYS check the address and amount you send out, your Dash can't be stolen or redirected. Dash transactions are however irreversable and noone can help you if you send something to the wrong address.
