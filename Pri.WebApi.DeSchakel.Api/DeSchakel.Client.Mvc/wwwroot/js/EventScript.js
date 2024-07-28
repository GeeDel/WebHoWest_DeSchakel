
function confirmDelete(uniqueId, isDeleteClicked)
{
            var deleteSpan = 'deleteSpan_' + uniqueId;
            var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

        if (isDeleteClicked) {
            $('#' + deleteSpan).hide();
            $('#' + confirmDeleteSpan).show();
        }
        else
        {
            $('#' + deleteSpan).show();
            $('#' + confirmDeleteSpan).hide();
        }
    }

const baseUrl = "https://localhost:44326/api";
const dbPerformances = fetch(`${baseUrl}/events`)
    .then((response) => response.json())
    .then((data) => {
        performances = data;
        showCategories();
    })
    .catch(error => console.log(error));