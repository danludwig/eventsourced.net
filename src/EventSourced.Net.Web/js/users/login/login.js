import {
  setFocus,
  serializeFormData,
  clearValidationErrors,
  disableForm,
  enableForm,
  setLocation,
  handleValidationErrors
} from '../../utils';

import $ from 'jquery';

const validationMessages = {
  login: {
    empty: 'Email or phone is required.',
    unverified: 'Invalid login or password.'
  },
  password: {
    empty: 'Password is required.'
  }
};

$(document).on('ready', function () {
  var formId = 'login_form', login = 'login';
  setFocus(formId, login);
  $('#' + formId).on('submit', function () {
    var $form = $(this), data = serializeFormData($form);
    clearValidationErrors($form);
    disableForm($form);
    $.ajax({
      type: 'POST',
      url: $form.attr('action'),
      data: data
    })
      .done(function (response, status, xhr) {
        setLocation(xhr);
      })
      .fail(function (xhr) {
        enableForm($form);
        handleValidationErrors(xhr, $form, validationMessages);
      });
    return false;
  });
});
