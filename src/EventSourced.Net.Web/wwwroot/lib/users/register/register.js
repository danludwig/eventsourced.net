import $ from 'jquery';
import {
  setFocus,
  serializeFormData,
  clearValidationErrors,
  disableForm,
  setLocation,
  enableForm,
  handleValidationErrors
} from '../../utils';

const validationMessages = {
  emailOrPhone: {
    empty: 'Email address or phone number is required.',
    invalidFormat: '**{attemptedValue}** does not appear to be a valid email address or US phone number.',
    alreadyExists: '**{attemptedValue}** has already been registered. Did you mean to log in?'
  },
  principal: {
    notEmpty: 'You are already logged in. Please log off to register a new user account.'
  }
};

export default function register() {
  var formId = 'register_form', emailOrPhone = 'emailOrPhone';
  setFocus(formId, emailOrPhone);
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
}
