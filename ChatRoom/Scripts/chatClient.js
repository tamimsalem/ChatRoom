function Room() {

    this._currentChater = '';
    this._initiated = false;

    this._chatHub = null;

    this.uiElements = {

        messagesContainer: $('#discussion'),
        txtMessage: $('#txtMessage'),
        btnSend: $('#btnSend'),
        spnName: $('#spnName')
    }
}


Room.prototype.start = function () {

    var self = this;

    var name = prompt('Tell us your name to start', '');

    if (name) {
        self._init(name);
    }
}

Room.prototype._init = function (currnetName) {

    var self = this;

    self._currentChater = currnetName;

    self.uiElements.spnName.text(currnetName);
    
    self._chatHub = $.connection.chatHub;

    self._chatHub.client.broadcastMessage = function (name, message) {

        self._appendMessage(name, message);
    };

    self._chatHub.client.showError = function (errorMessage) {

        self._showError(errorMessage);
    };

    self.uiElements.txtMessage.focus();

    $.connection.hub.start().done(function () {

        self.uiElements.btnSend.click(function () {

            var message = self.uiElements.txtMessage.val();

            if (message) {
                self._chatHub.server.send(self._currentChater, message);

                self.uiElements.txtMessage.val('').focus();
            }
            else {
                alert('Please write a message');
            }
        });

        self._initiated = true;
    });
}


Room.prototype._appendMessage = function (name, message) {

    var self = this;

    var encodedName = $('<div />').text(name).html();
    var encodedMsg = $('<div />').text(message).html();

    var liDir = 'left';

    if (name == self._currentChater) {
        liDir = 'right';
    }
    
    self.uiElements.messagesContainer.append('<li style=";float: ' + liDir + '"><strong>' + encodedName + '</strong><br />' + encodedMsg + '</li>');
}

Room.prototype._showError = function (errorMessage) {
    alert(errorMessage);
}


$(function () {

    var room = new Room();

    room.start();
});
