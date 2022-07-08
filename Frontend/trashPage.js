const constants = {
    apiBasePath: 'http://localhost:59689/api/Folder/'
  }


  //function for listing folders
function listFolders() {
    try
    {
      var create = document.getElementById("create");
      create.innerHTML = '';
    fetch(`http://localhost:59689/api/Folder/ShowTrash/`+sessionStorage.getItem("userid"), {
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
     <div class="favIcon"><img class="favImage" src="./unfavourite.png"></div>
     <div class="folderbtn"><img class="folderimg" onclick ="createfiles(${folderid})" src="./folder.png" '></div>
     <div class="btnText">
       <button id="filebtn" onclick ="createfiles(${folderid})" style="text-decoration: none;border: 0px;font-weight:bold; background: none"> ${fname} </button></div>
      <div class="footerIcons">
      <div class="infoBox"><img onclick='popup1(${folder.folderId})' class="infoIcon" src="./undo1.png"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folderid})'"></div>
      </div>
      </div>`
      create.appendChild(art);
      });
    
    })}
    catch(err)
    {
      console.log(err);
    }
  } 

  //function for creating files
  function createfiles(folderid){
  sessionStorage.setItem("folderId",folderid);
  window.location.href="file.html";
  }
  

  
  function onLoad() {
    listFolders();
    document.getElementById("adminName").innerHTML="Hi, "+sessionStorage.getItem("username") + "!";
  }
  
   
  onLoad();


  //for deleting the folder
   function popup(folderid) {
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
                 deletefolder(folderid),
                'File deleted successfully',
                'Your file has been deleted.',
                'success'
            )
        }
    })
  }


  //for undeleting a folder
  function popup1(folderid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You are restoring the folder!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, restore it!'
  
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                undelete(folderid),
                'File restored successfully',
                'Your file has been restored.',
                'success'
            )
  
        }
  
    })
  
  }



// hard delete folder
function deletefolder(folder) {
  var raw = "";
  var requestOptions = {
  method: 'DELETE',
  body: raw,
  redirect: 'follow'
};

let deleteurl = "http://localhost:59689/api/Folder/" + folder;
fetch(deleteurl,requestOptions)
.then(response=>response.text())
.then(result => console.log(listFolders()))
  .catch(error => console.log('error', error));  
}


// Undelete a folder/files
function undelete(folderId){
    var requestOptions = {
    method: 'PUT',
    redirect: 'follow'
    };
  
    fetch("http://localhost:59689/api/Folder/Undelete/"+folderId, requestOptions)
    .then(response => response.text())
    .then(result =>{
    console.log(result)
    listFolders()})
    .catch(error => console.log('error', error));
}



function searchItem(){

    try
    {
      var search=document.getElementById("search").value;
      var create = document.getElementById("create");
      create.innerHTML = '';
  
    fetch(`http://localhost:59689/api/Folder/SearchInTrash/${sessionStorage.getItem("userid")}/${search}`)
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
     <div class="favIcon"><img class="favImage" src="./unfavourite.png"></div>
     <div class="folderbtn"><img class="folderimg" onclick ="createfiles(${folderid})" src="./folder.png" '></div>
     <div class="btnText">
       <button id="filebtn" onclick ="createfiles(${folderid})" style="text-decoration: none;border: 0px;font-weight:bold; background: none"> ${fname} </button></div>
      <div class="footerIcons">
      <div class="infoBox"><img onclick='popup1(${folder.folderId})' class="infoIcon" src="./undo1.png"></div>
      <div class="trashIcon"><img class="delIcon" src="./delete.png" onclick ='popup(${folderid})'"></div>
      </div>
      </div>`;
        ;
        create.appendChild(art);
      });
    })
    }
  
    catch(err)
    {
      console.log(err);
    }
  }


  //function for viewing details
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