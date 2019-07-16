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


$(() => {
	let connection =
		new signalR.HubConnectionBuilder()
			.withUrl("/signalServer")
			.build()
	;
	connection.start();
	connection.on("refreshEmployees", function () {
		loadData();
	});
	loadData();
	function loadData() {
		var tr = '';
		$.ajax({
			url: '/Home/GetEmployees',
			method: 'GET',
			success: (result) => {
				$.each(result, (k, v) => {
					tr = tr +
						`<tr>
							<td>${v.id}</td>
							<td>${v.name}</td>
							<td>${v.age}</td>
						</tr>`
					;
				});
				$("#tableBody").html(tr);
			},
			error: (error) => {
				console.log(error);
			}
		});
	}
});
