
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

//If the client doesn't successfully reconnect within its first four attempts, the HubConnection will transition to the Disconnected state and fire its onclose callbacks. This provides an opportunity to inform users the connection has been permanently lost and recommend refreshing the page:
connection.onclose((error) => {
	console.assert(connection.state === signalR.HubConnectionState.Disconnected);

	document.getElementById("messageInput").disabled = true;

	const li = document.createElement("li");
	li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
	document.getElementById("messagesList").appendChild(li);
});

//In order to configure a custom number of reconnect attempts before disconnecting or change the reconnect timing, withAutomaticReconnect accepts an array of numbers representing the delay in milliseconds to wait before starting each reconnect attempt.
const connection4 = new signalR.HubConnectionBuilder()
	.withUrl("/chatHub")
	.withAutomaticReconnect([0, 0, 10000])
	.build();
// .withAutomaticReconnect([0, 2000, 10000, 30000]) yields the default behavior

const connection5 = new signalR.HubConnectionBuilder()
	.withUrl("/chatHub")
	.withAutomaticReconnect({
		nextRetryDelayInMilliseconds: (previousRetryCount, elapsedMilliseconds) => {
			if (elapsedMilliseconds < 60000) {
				// If we've been reconnecting for less than 60 seconds so far,
				// wait between 0 and 10 seconds before the next reconnect attempt.
				return Math.random() * 10000;
			} else {
				// If we've been reconnecting for more than 60 seconds so far, stop reconnecting.
				return null;
			}
		}
	})
	.build();

//Manually reconnect
async function start2() {
	try {
		await connection.start();
		console.log("connected");
	} catch (err) {
		console.log(err);
		setTimeout(() => start2(), 5000);
	}
};
connection.onclose(async () => {
	await start2();
});


//Logging
let connection6 = new signalR.HubConnectionBuilder()
	.withUrl("/myhub")
	.configureLogging(signalR.LogLevel.Information)
	.build();

let connection7 = new signalR.HubConnectionBuilder()
	.withUrl("/myhub")
	.configureLogging("warn")
	.build();

var connection8 = new HubConnectionBuilder()
	.WithUrl("https://example.com/myhub", HttpTransportType.WebSockets | HttpTransportType.LongPolling)
	.Build();

let connection9 = new signalR.HubConnectionBuilder()
	.withUrl("/myhub", { transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling })
	.build();


//Configure bearer authentication
let connection10 = new signalR.HubConnectionBuilder()
	.withUrl("/myhub", {
		accessTokenFactory: () => {
			// Get and return the access token.
			// This function can return a JavaScript Promise if asynchronous
			// logic is required to retrieve the access token.
		}
	})
	.build();

//Configure additional options
let connection11 = new signalR.HubConnectionBuilder()
	.withUrl("/myhub", {
		skipNegotiation: true,
		transport: signalR.HttpTransportType.WebSockets
	})
	.build();

