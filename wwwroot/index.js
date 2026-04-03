function setupPage() {
    const url = new URL(window.location);
    const path = url.searchParams.get("path");

    if (path == null || path.length == 0) { return; }
    updateLocation(path);
}

function UpdateFunction() {
    var inputLocation = document.getElementById("locationInput").value;
    updateLocation(inputLocation);
}

async function updateLocation(inputLocation, filter) {
    if (filter == null) { filter = ""; }
    try {
        if (inputLocation == null) { inputLocation = ""; }
        var response = await fetch('/Test?RequestType=updateLocation&location=' + inputLocation + "&filter=" + filter);

        //Check for bad Request
        if (!response.ok) {
            alert("Bad Request")
            throw new Error(`HTTP error: ${response.status}`);
            return;
        }

        const RespDTO = await response.json();
        var result = JSON.parse(RespDTO.data);
        var files = result.DataList;

        //Update Query String
        const url = new URL(window.location);
        url.searchParams.set("path", result.FullPath);
        history.pushState(null, '', url);

        document.getElementById("locationInput").value = result.FullPath;

        //Build Rows
        var displayText = "";
        for (let i = 0; i < files.length; i++) {
            displayText += buildRow(i, files[i].FileName, files[i].FileSize, result.FullPath);
        }
        document.getElementById("tableBody").innerHTML = displayText;
        document.getElementById("fileCount").innerHTML = result.FileCount;
        document.getElementById("folderCount").innerHTML = result.FolderCount;

        console.log(data);

    } catch (error) {
        console.error('Error:', error);
    }

}


function buildRow(index, fileName, fileSize, path) {
    var background = "";
    if (index % 2 == 1) { background = "#F0F0F0" }

    var extraInfo = "";
    if (fileSize.length > 0) { extraInfo = fileSize + " Bytes"; }

    var navigateTo = "";
    if (fileSize.length == 0) { navigateTo = "navigateToFolder('" + fileName + "')"; }
    else { navigateTo = "navigateToFile('" + fileName + "')"; }

    var downloadTxt = "";
    if (fileSize.length > 0) {
        downloadTxt = "<a href=\"" + path + "\\" + fileName + "\" download)\">Download</a>";
    }

    var highlightColor = "#e0e0e0";
    var row = "<tr style=\"cursor:pointer;background:" + background + "\" ondblclick=\"" + navigateTo + "\""
        + " onmouseover =\"this.style.backgroundColor='" + highlightColor + "'\" onmouseout=\"this.style.backgroundColor='" + background + "'\">"
        + "<td>" + fileName + "</td>"
        + "<td>" + extraInfo + "</td>"
        + "<td>" + downloadTxt + "</td>"
        + "</tr> ";

    return row;
}

function navigateToFile(name) {
    alert(name + " has been selected");
}

function navigateToFolder(name) {
    const url = new URL(window.location);
    var currentPath = url.searchParams.get("path");
    updateLocation(currentPath + "\\" + name);
}

function goUpDirectory() {
    const url = new URL(window.location);
    var currentPath = url.searchParams.get("path");
    var idx = currentPath.lastIndexOf("\\");
    if (idx !== -1) {
        var result = currentPath.substring(0, idx);
        updateLocation(result);
    }
}


async function uploadFile() {
    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];
    if (!file) {
        alert("Please select a file");
        return;
    }
    const url = new URL(window.location);
    var currentPath = url.searchParams.get("path");

    const formData = new FormData();
    formData.append("currentPath", currentPath);
    formData.append("myFile", file);

    try {
        const response = await fetch('/Test', { method: 'POST', body: formData });

        if (response.ok) {
            console.log("UploadSuccessful!");
            document.getElementById('fileInput').value = "";
            updateLocation(currentPath);
        }
        else {
            console.log("Uplaod Failed.");
        }
    }
    catch (error) {
        console.error('Error:', error);
    }

}


function searchFunction() {
    const searchInput = document.getElementById('searchInput').value;
    
    const url = new URL(window.location);
    var currentPath = url.searchParams.get("path");
    updateLocation(currentPath, searchInput);
}