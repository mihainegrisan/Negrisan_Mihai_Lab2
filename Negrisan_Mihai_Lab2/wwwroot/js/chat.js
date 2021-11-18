"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (firstName, lastName, message, dateTime) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = dateTime + ": \"" + firstName + " " + lastName + "\" says " + msg;
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
    var firstName = document.getElementById("userFirstNameInput").value;
    var lastName = document.getElementById("userLastNameInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", firstName, lastName, message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});