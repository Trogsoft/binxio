const rt = new function () {

    const r = this;

    var tasksStarted = 0;
    var connection = new signalR.HubConnectionBuilder().withUrl("/rt").build();

    connection.start().then(function () {
        console.log('connected.');
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("taskBegin", function (data) {
        $('body').append('<div class="task-alert" id="task-alert-' + data.operationId + '"><div class="title">' + data.taskTitle + '</div><div>Task is running in the background.</div></div>');
        setTimeout(function () {
            $('#task-alert-' + data.operationId).remove();
        }, 5000);
        tasksStarted++;
        $('.tasks-btn i').addClass('fa-spin');
    });

    connection.on("taskStatus", function (data) {
        console.log('taskStatus: ' + data);
    });

    connection.on("taskComplete", function (data) {
        $('body').append('<div class="task-alert" id="task-alert-' + data.operationId + '"><div class="title">' + data.taskTitle + '</div><div>Task completed.</div></div>');
        setTimeout(function () {
            $('#task-alert-' + data.operationId).remove();
        }, 5000);
        tasksStarted--;
        if (tasksStarted < 1) {
            tasksStarted = 0;
            $('.tasks-btn i').removeClass('fa-spin');
        }
    });

    connection.on("taskProgress", function (data) {
        console.log('taskProgress: ' + data);
    });

    return r;

}

////Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;



//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});