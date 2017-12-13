var files = document.getElementById('FileUpload').files;
var AttachmentData = = new FormData();
for (var i = 0; i < files.length; i++) {
  AttachmentData.append(files[i].name, files[i]);
}
AttachmentData.append('EmailFrom', document.getElementById('txtSendFrom').value);
AttachmentData.append('EmailTo', document.getElementById('txtSendTo').value);
AttachmentData.append('EmailCC', document.getElementById('txtSendCC').value);
AttachmentData.append('EmailBCC', document.getElementById('txtSendBCC').value);
AttachmentData.append('EmailSubject', document.getElementById('txtSubject').value);
AttachmentData.append('EmailMessage', document.getElementById('txtMessage').value);

$.ajax({
        url: '/EmailAttachment/SendEmail',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data
        data: AttachmentData,
        success: function (result) {
            alert(result);
        },
        error: function (err) {
          alert(err);
        }
    });