//Manage Announcements

const confirmAnnouncementDelete = (announcementName) => {
    //e.preventDefault();
    swal({
        title: "Delete Announcement",
        text: `Are you sure you want to delete announcement with name: ${announcementName}?`,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $("#deleteAnnouncementForm").submit();
            return true;
        }
    });
    return false;
}