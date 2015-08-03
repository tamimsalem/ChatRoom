function Room() {

    this._currentChater = {};
    this._initiated = false;

    this._chatHub = null;

    this.uiElements = {

        messagesContainer: $('#discussion'),
        txtMessage: $('#txtMessage'),
        btnSend: $('#btnSend'),
        spnName: $('#spnName'),
        userList: $('#userList')
    }
}

Room.prototype.start = function () {

    var self = this;

    var name = prompt('Tell us your name to start', '');

    if (name) {
        self._init(name);
    }
}

Room.prototype._guid = function() {

    function s4() {

        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }

    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}


Room.prototype._init = function (currnetName) {

    var self = this;

    self._currentChater = {
        id: self._guid(),
        name: currnetName
    };

    self.uiElements.spnName.text(currnetName);
    
    self._chatHub = $.connection.chatHub;

    self._chatHub.client.broadcastMessage = function (name, message) {

        self._appendMessage(name, message);
    };

    self._chatHub.client.joined = function (user) {

        self._addToUserList(user);
    };

    self._chatHub.client.left = function (user) {

        self._removeFromUserList(user);
    };

    self._chatHub.client.showError = function (errorMessage) {

        self._showError(errorMessage);
    };

    self._chatHub.client.load = function (allOthers) {

        self._load(allOthers);
    };


    self.uiElements.txtMessage.focus();

    $.connection.hub.qs = { id: self._currentChater.id, name: self._currentChater.name };

    $.connection.hub.start().done(function () {

        self.uiElements.btnSend.click(function () {

            var message = self.uiElements.txtMessage.val();

            if (message) {
                self._chatHub.server.send(self._currentChater.name, message);

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

    if (name == self._currentChater.name) {
        liDir = 'right';
    }
    
    self.uiElements.messagesContainer.append('<li style=";float: ' + liDir + '"><strong>' + encodedName + '</strong><br />' + encodedMsg + '</li>');
}

Room.prototype._showError = function (errorMessage) {
    alert(errorMessage);
}

Room.prototype._removeFromUserList = function (user) {
    var self = this;

    var item = $('[data-id=' + user.Id + ']');

    item.remove();
}

Room.prototype._addToUserList = function (user) {
    var self = this;

    self.uiElements.userList.append('<li data-id=' + user.Id + '>' + user.Name + '</li>');
}

Room.prototype._load = function (allOthers) {

    var self = this;

    $.each(allOthers, function (index, value) {
        self.uiElements.userList.append('<li data-id=' + value.Id + '>' + value.Name + '</li>');
    });
}


$(function () {

    var room = new Room();

    room.start();
});
