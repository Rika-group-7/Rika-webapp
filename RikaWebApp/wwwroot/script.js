const toggleSearch = () => {
    const searchForm = document.querySelector('.search-form')
    const searchInput = document.querySelector('.search-input')
    const searchButton = document.querySelector('.search-button')

    //Kontroll av Elementens Existens
    if (searchForm && searchInput && searchButton) {  
        searchButton.addEventListener('click', () => {
            searchForm.classList.toggle('active-search');
            console.log(searchForm.classList.contains('active-search'));
        });

        searchInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                searchInput.value = '';
                searchForm.classList.remove('active-search');
            }
        });
    }
};

toggleSearch();