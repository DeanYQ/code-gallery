function createAjaxRequeset() {
    try {
        request = new XMLHttpRequest();
    }
    catch(e) {
        console.log(e.message);
        request = null;
    }
    return request;
}