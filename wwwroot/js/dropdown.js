window.dropdownInterop = {
    registerOutsideClick: function (element, dotnetHelper) {

        document.addEventListener('click', function (event) {

            if (!element.contains(event.target)) {
                dotnetHelper.invokeMethodAsync('CloseDropdown');
            }

        });

    }
};