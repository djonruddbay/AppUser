
var profileInput = document.querySelector('#imgProfileUpload');
var profilePreview = document.querySelector('#imgProfilePreview');
var profileFileTypes = [
    'image/jpeg',
    'image/pjpeg',
    'image/png'
]

profileInput.style.height = 0;
profileInput.addEventListener('change', changeImageDisplay);

function changeImageDisplay() {
    if (profileInput.files.length > 0 && validProfileImageFileType(profileInput.files[0])) {
        updateImageDisplay();
    }
}
function updateImageDisplay() {
    while (profilePreview.firstChild) {
        profilePreview.removeChild(profilePreview.firstChild);
    }
    var image = document.createElement('img');
    image.src = window.URL.createObjectURL(profileInput.files[0]);
    profilePreview.appendChild(image);
    image.classList = "ProfileImage";


    document.getElementById("btnSaveChanges").disabled = false;
}

function validProfileImageFileType(file) {
    for (var i = 0; i < profileFileTypes.length; i++) {
        if (file.type === profileFileTypes[i]) {
            return true;
        }
    }
    return false;
}
