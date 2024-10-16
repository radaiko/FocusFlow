function setAccessToken(token) {
    localStorage.setItem('googleAccessToken', token);
}

async function getFiles() {
    const accessToken = localStorage.getItem('googleAccessToken');
    if (!accessToken) {
        console.error("Access token is not available.");
        return;
    }

    const response = await fetch('https://www.googleapis.com/drive/v3/files?spaces=appDataFolder&fields=nextPageToken,files(id,name)', {
        headers: {
            'Authorization': 'Bearer ' + accessToken
        }
    });

    if (!response.ok) {
        console.error("Error listing files: ", response.statusText);
        return;
    }

    const data = await response.json();
    return data.files;
}

async function getFile(id){
    const accessToken = localStorage.getItem('googleAccessToken');
    if (!accessToken) {
        console.error("Access token is not available.");
        return;
    }

    const response = await fetch('https://www.googleapis.com/drive/v3/files/' + id + '?alt=media', {
        headers: {
            'Authorization': 'Bearer ' + accessToken
        }
    });

    if (!response.ok) {
        console.error("Error getting file: ", response.statusText);
        return;
    }

    return await response.text();
}

async function getFileContent(id) {
    const accessToken = localStorage.getItem('googleAccessToken');
    if (!accessToken) {
        console.error("Access token is not available.");
        return;
    }

    const response = await fetch('https://www.googleapis.com/drive/v3/files/' + id + '?alt=media', {
        headers: {
            'Authorization': 'Bearer ' + accessToken
        }
    });

    if (!response.ok) {
        console.error("Error getting file content: ", response.statusText);
        return;
    }

    return await response.text();
}

async function uploadFile(fileContent, fileName) {
    const accessToken = localStorage.getItem('googleAccessToken');
    if (!accessToken) {
        console.error("Access token is not available.");
        return;
    }

    var file = new Blob([fileContent], { type: 'text/plain' });
    var metadata = {
        'name': fileName,
        'parents': ['appDataFolder'],
        'mimeType': 'text/plain'
    };

    var form = new FormData();
    form.append('metadata', new Blob([JSON.stringify(metadata)], { type: 'application/json' }));
    form.append('file', file);

    const response = await fetch('https://www.googleapis.com/upload/drive/v3/files?uploadType=multipart&fields=id', {
        method: 'POST',
        headers: new Headers({ 'Authorization': 'Bearer ' + accessToken }),
        body: form
    });

    if (!response.ok) {
        console.error("Error uploading file: ", response.statusText);
        return;
    }

    const data = await response.json();
    return data.id;
}

async function deleteFile(id) {
    const accessToken = localStorage.getItem('googleAccessToken');
    if (!accessToken) {
        console.error("Access token is not available.");
        return;
    }

    const response = await fetch('https://www.googleapis.com/drive/v3/files/' + id, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Bearer ' + accessToken
        }
    });

    if (!response.ok) {
        console.error("Error deleting file: ", response.statusText);
    }
}
