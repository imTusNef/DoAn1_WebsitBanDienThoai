var cart = {
    init: function () {
        cart.regEvents();
    },

    regEvents: function () {
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/Delete',
                data: { id: id },
                type: 'POST',
                success: function (response) {
                    if (response.status == true) {
                        location.reload(); // reload lại giỏ hàng sau khi xóa
                    }
                }
            });
        });
    }
}
cart.init();