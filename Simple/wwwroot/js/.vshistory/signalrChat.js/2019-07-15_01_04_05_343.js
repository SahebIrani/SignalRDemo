


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

