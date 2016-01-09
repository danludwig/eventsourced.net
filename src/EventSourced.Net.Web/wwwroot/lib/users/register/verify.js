import $ from 'jquery'
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
  code: {
    empty: 'Secret code is required.',
    unverified: 'Invalid code. You can try {codeAttemptsRemainingCount} more time(s).',
    maxAttempts: 'You have exceeded the maximum allowable secret code attempts.',
    stateConflict: 'Your code has already been validated. Please use the forward button on your browser and do not navigate back to this page.'
  },
  correlationId: {
    'null': 'Something went wrong. Please restart the registration process.'
  }
};

export default function verify() {
  var formId = 'verify_form', code = 'code';
  setFocus(formId, code);
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
