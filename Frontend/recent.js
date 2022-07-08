const constants = {
    apiBasePath: 'http://localhost:59689/api/Folder/Recent/'
  }
 
  const form = document.getElementById("input1");
  console.log(form);
  var curr=new Date();
//function for creating folders
  function createFolders() {
    try
    {
    
     fetch(`${constants.apiBasePath}Folder`, {
       body: JSON.stringify(
      {
        "folderName": form.value,
        "folderCreatedBy" : sessionStorage.getItem("userid"),
        "folderCreatedAt":curr.toISOString(),
        "folderIsDeleted": 0
    }
      
      ),
       method: 'POST',
       headers: {
        'Content-Type': 'application/json'
      },
     }).then((folderCreateResponse) => {
        console.log(folderCreateResponse);
         listFolders();
     });
    }
    catch(err)
    {
      console.log(err);
    }
  }
  // function for listing folders
  function listFolders() {
    try
    {
      var create = document.getElementById("create");
      var date=new Date();
      console.log("hello"+date.getHours());
      create.innerHTML = '';
    fetch("http://localhost:59689/api/Folder/ShowRecentFolders/"+sessionStorage.getItem("userid")+"/"+date.getHours(),
    {
      method: 'GET'
    })
    .then(response => response.json())
    .then((folders) => {
     
    folders.forEach(folder => {
   
      var create = document.getElementById("create");
      var art = document.createElement("article");
      art.setAttribute("id","section");
      const fname = folder.folderName;
      const folderid=folder.folderId;
 
      
      console.log(folderid);
      art.innerHTML =
     `
     <div class="folderBox">
     <div class="favIcon"><img class="favImage" src="./unfavourite.png" onclick="favourite(${folderid})"></div>
     <div class="folderbtn"><img class="folderimg" onclick ="createfiles(${folderid})" src="./folder.png" '></div>
     <div class="btnText">
       <button id="filebtn" onclick ="createfiles(${folderid})" style="text-decoration: none;border: 0px;font-weight:bold; background: none"> ${fname} </button></div>
      <div class="footerIcons">
      <div class="infoBox"><img onclick='opendetails(${folder.folderId},"${folder.folderName}","${folder.folderCreatedBy}","${folder.folderCreatedAt}")' class="infoIcon" src="./info.png"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folderid})'"></div>
      </div>
      </div>`;  

      create.appendChild(art);
      });
    
    })}
    catch(err)
    {
      console.log(err);
    }
  } 

//function for favouriting folders
function favourite(folderId){
  var requestOptions = {
    method: 'PUT',
    redirect: 'follow'
  };

  fetch("http://localhost:59689/api/Folder/Favourite/"+folderId, requestOptions)
    .then(response => response.text())
    .then(result =>{
      console.log(result)
      listFolders()})
    .catch(error => console.log('error', error));
}



//function for creating files
  function createfiles(folderid){
  sessionStorage.setItem("folderid",folderid);
  window.location.href="file.html";
  }
  
  function onLoad() {
    listFolders();
    document.getElementById("adminName").innerHTML="Hi, "+sessionStorage.getItem("username") + "!";
  }
  
   onLoad();

//function for showing sweet alert on deleting
   function popup(folderid) {
  Swal.fire({
      title: 'Are you sure?',
      text: "You can still find the file in trash!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'

  }).then((result) => {
      if (result.isConfirmed) {
          Swal.fire(
               sendToTrash(folderid),
              
              'File removed successfully',
              'Your file has been deleted.',
              'success'
          )
      }
  })

}

//  file path 
const next = document.getElementById('hello');
console.log(next);

  

//function for logging out
function logout(){
  sessionStorage.clear();
  window.location.href="index.html";
}


//function for searching
function searchItem(){

  try
  {
    var search=document.getElementById("search").value;
    var create = document.getElementById("create");
    create.innerHTML = '';

  fetch(`http://localhost:59689/api/Folder/${sessionStorage.getItem("userid")}/${search}`)
  .then(response => response.json())
  .then((folders) => {
    console.log(folders);
    folders.forEach(folder => {
      var create = document.getElementById("create");
      var art = document.createElement("article");
      art.setAttribute("id","section");
      const fname = folder.folderName;
      const folderid = folder.folderId;

     
      art.innerHTML =
      `
     <div class="folderBox">
     <div class="favIcon"><img class="favImage" src="./unfavourite.png onclick="favourite(${folderid})"></div>
     <div class="folderbtn"><img class="folderimg" onclick ="createfiles(${folderid})" src="./folder.png" '></div>
     <div class="btnText">
       <button id="filebtn" onclick ="createfiles(${folderid})" style="text-decoration: none;border: 0px;font-weight:bold; background: none"> ${fname} </button></div>
      <div class="footerIcons">
      <div class="infoBox"><img onclick='opendetails(${folder.folderId},"${folder.folderName}","${folder.folderCreatedBy}","${folder.folderCreatedAt}")' class="infoIcon" src="./info.png"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folderid})'"></div>
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


//soft delete 
function sendToTrash(folderId){
  var requestOptions = {
    method: 'PUT',
    redirect: 'follow'
  };

  fetch("http://localhost:59689/api/Folder/SoftDelete/"+folderId, requestOptions)
    .then(response => response.text())
    .then(result =>{
      console.log(result)
      listFolders()})
    .catch(error => console.log('error', error));
}

