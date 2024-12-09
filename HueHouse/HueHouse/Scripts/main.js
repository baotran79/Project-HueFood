document.addEventListener('DOMContentLoaded', function () {
    // Xử lý menu và chuyển đổi giữa các danh mục
    const menu = document.querySelector('.horizontal-menu');
    const menuSections = document.querySelectorAll('.menu-section');

    if (menu) {
        menu.addEventListener('click', function (event) {
            const clickedItem = event.target.closest('li');
            if (clickedItem) {
                // Xóa class 'active' khỏi tất cả các mục menu
                const menuItems = menu.querySelectorAll('li');
                menuItems.forEach(item => item.classList.remove('active'));

                // Thêm class 'active' vào mục được chọn
                clickedItem.classList.add('active');

                // Ẩn tất cả các phần nội dung
                menuSections.forEach(section => section.style.display = 'none');

                // Hiển thị phần nội dung tương ứng
                const category = clickedItem.getAttribute('data-category');
                const activeSection = document.getElementById(category);
                if (activeSection) {
                    activeSection.style.display = 'block';
                }
            }
        });

        // Hiển thị danh mục mặc định khi tải trang
        const defaultCategory = menu.querySelector('li[data-category]');
        if (defaultCategory) {
            defaultCategory.click();
        }
    }

    // Xử lý tìm kiếm món ăn
    const searchQueryInput = document.getElementById('searchQuery');
    const searchResultsDiv = document.getElementById('searchResults');

    if (searchQueryInput) {
        searchQueryInput.addEventListener('input', function () {
            const query = searchQueryInput.value.trim();

            if (query.length > 0) {
                // Gửi yêu cầu AJAX đến controller
                fetch(`/Home/Search?query=${encodeURIComponent(query)}`)
                    .then(response => response.json())
                    .then(data => {
                        // Hiển thị kết quả tìm kiếm
                        if (data.length > 0) {
                            searchResultsDiv.innerHTML = ''; // Xóa kết quả cũ
                            data.forEach(item => {
                                const resultItem = document.createElement('div');
                                resultItem.classList.add('search-result-item');

                                const img = document.createElement('img');
                                img.src = item.ImageUrl; // Đường dẫn đến hình ảnh của món ăn
                                img.alt = item.FoodName;

                                const name = document.createElement('span');
                                name.textContent = item.name; // Tên món ăn

                                resultItem.appendChild(img);
                                resultItem.appendChild(name);
                                searchResultsDiv.appendChild(resultItem);
                            });
                            searchResultsDiv.style.display = 'block';
                        } else {
                            searchResultsDiv.style.display = 'none'; // Nếu không có kết quả, ẩn dropdown
                        }
                    })
                    .catch(error => {
                        console.error('Lỗi khi tìm kiếm:', error);
                    });
            } else {
                searchResultsDiv.style.display = 'none'; // Nếu ô tìm kiếm trống, ẩn dropdown
            }
        });
    }
});