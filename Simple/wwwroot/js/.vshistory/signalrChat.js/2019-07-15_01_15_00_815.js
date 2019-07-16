


connection.invoke("GetTotalLength", "value1");
connection.invoke("GetTotalLength", { param1: "value1" });
connection.invoke("GetTotalLength", { param1: "value1", param2: "value2" });
connection.on("ReceiveMessage", (req) => {
	appendMessageToChatWindow(req.message);
});
connection.on("ReceiveMessage", (req) => {
	let message = req.message;
	if (req.sender) {
		message = req.sender + ": " + message;
	}
	appendMessageToChatWindow(message);
});

//Connect to a hub
const connection = new signalR.HubConnectionBuilder()
	.withUrl("/chatHub")
	.configureLogging(signalR.LogLevel.Information)
	.build();

connection.start().then(function () {
	console.log("connected");
});

//Call hub methods from client
connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));

//Call client methods from hub
connection.on("ReceiveMessage", (user, message) => {
	const encodedMsg = user + " says " + message;
	const li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});

//public async Task SendMessage(string user, string message)
//{
//	await Clients.All.SendAsync("ReceiveMessage", user, message);
//}

//Error handling and logging
connection.start().catch(function (err) {
	return console.error(err.toString());
});
//signalR.LogLevel.Error – Error messages.Logs Error messages only.
//signalR.LogLevel.Warning – Warning messages about potential errors.Logs Warning, and Error messages.
//signalR.LogLevel.Information – Status messages without errors.Logs Information, Warning, and Error messages.
//signalR.LogLevel.Trace – Trace messages.Logs everything, including data transported between hub and client.
const connection2 = new signalR.HubConnectionBuilder()
	.withUrl("/chatHub")
	.configureLogging(signalR.LogLevel.Information)
	.build();

//Reconnect clients
const connection3 = new signalR.HubConnectionBuilder()
	.withUrl("/chatHub")
	.withAutomaticReconnect()
	//Without any parameters, withAutomaticReconnect() configures the client to wait 0, 2, 10, and 30 seconds respectively before trying each reconnect attempt, stopping after four failed attempts.
	.build();

connection.onreconnecting((error) => {
	console.assert(connection.state === signalR.HubConnectionState.Reconnecting);

	document.getElementById("messageInput").disabled = true;

	const li = document.createElement("li");
	li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
	document.getElementById("messagesList").appendChild(li);
});

//Since the connection looks entirely new to the server, a new connectionId will be provided to the onreconnected callback.
//The onreconnected callback's connectionId parameter will be undefined if the HubConnection was configured to skip negotiation.
connection.onreconnected((connectionId) => {
	console.assert(connection.state === signalR.HubConnectionState.Connected);

	document.getElementById("messageInput").disabled = false;

	const li = document.createElement("li");
	li.textContent = `Connection reestablished. Connected with connectionId "${connectionId}".`;
	document.getElementById("messagesList").appendChild(li);
});

//withAutomaticReconnect() won't configure the HubConnection to retry initial start failures, so start failures need to be handled manually:
async function start() {
	try {
		await connection.start();
		console.assert(connection.state === signalR.HubConnectionState.Connected);
		console.log("connected");
	} catch (err) {
		console.assert(connection.state === signalR.HubConnectionState.Disconnected);
		console.log(err);
		setTimeout(() => start(), 5000);
	}
};

