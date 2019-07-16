


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


