window.auth = {
    setToken: function (token) {
        localStorage.setItem('authToken', token);
    },
    removeToken: function () {
        localStorage.removeItem('authToken');
    },
    getToken: function () {
        return localStorage.getItem('authToken');
    }
};

window.saveAsFile = (filename, base64Content) => {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64Content;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

