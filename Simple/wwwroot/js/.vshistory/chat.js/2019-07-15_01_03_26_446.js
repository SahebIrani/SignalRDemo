"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = user + " says " + msg;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
	document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
	return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
	var user = document.getElementById("userInput").value;
	var message = document.getElementById("messageInput").value;
	connection.invoke("SendMessage", user, message).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});





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

