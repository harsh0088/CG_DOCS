const   apiBasePath = "http://localhost:59689/api/Document/";


var form=document.getElementById("input1");
var foldersId=sessionStorage.getItem("folderid");
var userid=sessionStorage.getItem("userid");
var curr=new Date();

//function for creating files
function createfile() {
    try
    {
     fetch('http://localhost:59689/api/Document', {
       body: JSON.stringify({
       
        "documentName": form.value,
        "documentContentType": "text",
        "documentSize": 100,
        "documentCreatedBy": sessionStorage.getItem("userid"),
        "documentCreatedAt": curr.toISOString(),
        "documentIsDeleted": false,
        "folderId": sessionStorage.getItem("folderid"),
          
      }),
       method: 'POST',
       headers: {
        'Content-Type': 'application/json'
      },
     }).then((folderCreateResponse) => {
         console.log(folderCreateResponse);
         listFile();
     });
    }
    catch(err)
    {
      console.log(err);
    }
    } 

    //function for listing files
      function listFile() {
        try
        {
          var create = document.getElementById("create");
          create.innerHTML = '';
        fetch('http://localhost:59689/api/Document/'+sessionStorage.getItem("folderid"), {
          method: 'GET'
        })
        .then(response => response.json())
        .then((folders) => {
          console.log(folders);
          folders.forEach(folder => {
        
          var create = document.getElementById("create");
          var art = document.createElement("article");
          console.log(folder);
          const fname = folder.documentName;
          art.innerHTML = `
          <div class="fileBox">
          <div class="favIcon"><img class="favImage" src="./unfavourite.png"></div>
          <div class="folderbtn"><img class="folderimg" src="./fileIcon.png" >
          </div>
          <div class="textBtn">
          <button id="filebtn" style="text-decoration: none;border: 0px;font-weight:bold; background: none;cursor:pointer;"> ${fname} </button></div>

          <div class="footerIcons">
          <div class="infoBox"><img class="infoIcon" src="./info.png" onclick='opendetails(${folder.documentId},"${folder.documentName}","${folder.documentCreatedBy}","${folder.documentCreatedAt}")' style="cursor:pointer;"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folder.documentId})'></div>
          </div>
          </div>
          `;  
          
          create.appendChild(art);
          });
        })
        
        }
        catch(err)
        {
          console.log(err);
        }
      }
      

      
  function onLoad() {
    listFile();
document.getElementById("adminName").innerHTML="Hi, "+sessionStorage.getItem("username") + "!";
  }
  
 onLoad();


 //function for showing sweet alert on deleting
 function popup(documentId) {

  Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'

  }).then((result) => {
      if (result.isConfirmed) {
          Swal.fire(
            deletefile(documentId),
              'File deleted successfully',
              'Your file has been deleted.',
              'success'
          )

      }

  })

}


//function for logging out
 function logout(){
  sessionStorage.clear();
  window.location.href="index.html";
}


//function for opening details
function opendetails(folderid,foldername,createdby,createdate){

  var folderdetails='';

  folderdetails+=`<p class='folderdetails'>Folder Name :  <span class='folderdetailstext'>`+foldername+`</span></p>`;

  folderdetails+=`<p class='folderdetails'>Folder Created By :  <span class='folderdetailstext'>`+sessionStorage.getItem("username")+`</span></p>`;

  folderdetails+=`<p class='folderdetails'>Folder Created At :  <span class='folderdetailstext'>`+createdate+`</span></p>`;

  Swal.fire({
    title: 'Details',
    html:folderdetails,
    showClass: {
      popup: 'animate__animated animate__fadeInDown'
    },
    hideClass: {
      popup: 'animate__animated animate__fadeOutUp'
    },
    confirmButtonColor: 'green',
  })
}


//delete file
function deletefile(documentId) {
  debugger
  var raw = "";
var requestOptions = {
  method: 'DELETE',
  body: raw,
  redirect: 'follow'
};

let deleteurl =  apiBasePath + documentId;
fetch(deleteurl,requestOptions)
.then(response=>response.text())
.then(result => console.log(listFile()))
  .catch(error => console.log('error', error));
    
}


//function for searching files

function searchItem(){

  try

  {

    var search=document.getElementById("search").value;
    var create = document.getElementById("create");

    create.innerHTML = '';

  fetch(`http://localhost:59689/api/Document/${sessionStorage.getItem("folderid")}/${search}`)

  .then(response => response.json())

  .then((folders) => {

    console.log(folders);

    folders.forEach(folder => {
      var create = document.getElementById("create");
      var art = document.createElement("article");
      art.setAttribute("id","section");
      const fname = folder.documentName;
      const folderid=folder.documentId;     

     
      console.log(folderid);
      art.innerHTML =
      `<div class="fileBox">
          <div class="favIcon"><img class="favImage" src="./favourite.png"></div>
          <div class="folderbtn"><img class="folderimg" src="./fileIcon.png" >
          </div>
          <div class="textBtn">
          <button id="filebtn" style="text-decoration: none;border: 0px;font-weight:bold; background: none;cursor:pointer;"> ${fname} </button></div>

          <div class="footerIcons">
          <div class="infoBox"><img class="infoIcon" src="./info.png" onclick='openfiledet(${folder.documentId},"${folder.documentName}","${folder.documentCreatedBy}","${folder.documentCreatedAt}")' style="cursor:pointer;"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folder.documentId})'></div>
          </div>
          </div>`;
      create.appendChild(art);
    });
  })
  }
  catch(err)

  {
    console.log(err);
  }

}


//  upload a the file from local storage 
function uploadfile() 
{
let choose = document.getElementById('input1');
  let value = choose.files[0];
var formdata = new FormData();

formdata.append("files", value);

var requestOptions = {
  method: 'POST',
  body: formdata,
  redirect: 'follow'

};
  fetch("http://localhost:59689/api/Document/upload/"+foldersId+"/"+userid+"/"+curr.toISOString(), requestOptions)
  .then(response => response.text())
  .then(result => {console.log(result)
  listFile()})
  .catch(error => console.log('error', error));

}
  
  

