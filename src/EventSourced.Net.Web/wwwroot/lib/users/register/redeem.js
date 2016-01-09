import $ from 'jquery';
import {
  serializeFormData,
  setFocus,
  disableForm,
  enableForm,
  getCorrelationSocket,
  parseMessageData,
  setLocation,
  getValidationMessage,
  addValidationError,
  handleValidationErrors,
  clearValidationErrors
} from '../../utils';

const validationMessages = {
  username: {
    empty: 'Username is required.',
    invalidFormat: 'Username can only contain letters, numbers, hyphens, underscores, dots, and must be between 2 and 12 characters long.',
    alreadyExists: 'Sorry, the username **{attemptedValue}** is already taken. Please choose a different username.',
    phoneNumber: 'You cannot use a phone number as your username.',
    success: 'You will also be able to login with the username above.'
  },
  password: {
    empty: 'Password is required.',
    notEqual: 'Passwords do not match.',
    invalidFormat: 'Password must contain at least {minCharacters} characters.'
  },
  passwordConfirmation: {
    empty: 'Password confirmation is required.'
  },
  token: {
    stateConflict: 'Your password has already been created. Please use the forward button on your browser and do not navigate back to this page.',
    unverified: 'Your opportunity to complete registration has expired. Please restart the registration process.'
  },
  correlationId: {
    'null': 'Something went wrong. Please restart the registration process.'
  },
  emailOrPhone: {
    alreadyExists: 'The login **{attemptedValue}** has already been registered. Did you forget your password?'
  }
};

export default function redeem() {
  var formId = 'redeem_form', username = 'username';
  setFocus(formId, username);
  function receiveCheckUsernameResponse($button, usernameValue, response) {
    var successOrError = response.isAvailable ? 'success' : 'error';
    var successOrDanger = response.isAvailable ? 'success' : 'danger';
    $button.parents('.form-group').addClass('has-' + successOrError);
    $button.find('.glyphicon').hide();
    $button.find('.glyphicon.text-' + successOrDanger).show();
    var reason = response.reasonInvalid;
    var $checkUsernameMessage = $('#check_username_message');
    var validationMessage = getValidationMessage({
      key: username,
      reason: reason || 'success',
      value: usernameValue
    }, validationMessages);
    $checkUsernameMessage.html(validationMessage);
    $button.parents('.form-group').find('.help-block').hide();
    $checkUsernameMessage.show();
  }
  $('#check_username').on('click', function () {
    var $button = $(this);
    var $form = $('#' + formId);
    var url = $button.data('click-action');
    var usernameValue = $form.find('[name=' + username + ']').val();
    disableForm($form);
    $button.parents('.form-group').removeClass('has-success').removeClass('has-error');
    $button.find('.glyphicon').hide();
    $button.find('.glyphicon.text-info').show();
    $button.parents('.form-group').find('.help-block').hide();
    $button.parents('.form-group').find('.help-info.checking-availability').show();
    $.ajax({
      type: 'POST',
      url: url,
      data: {
        username: usernameValue
      }
    })
      .done(function (response) {
        receiveCheckUsernameResponse($button, usernameValue, response);
      })
      .fail(function (xhr) {
        receiveCheckUsernameResponse($button, usernameValue, {
          reasonInvalid: 'status' + xhr.status
        });
      })
    .always(function () {
      enableForm($form);
      $form.find('[name=emailOrPhone]').attr('disabled', 'disabled');
    });
    return false;
  });

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
        var socket = getCorrelationSocket(xhr);
        var isConstraintViolated = false;
        socket.onopen = function (e) {
          console.log('WebSockets: Correlation ' + e.type);
        }
        socket.onmessage = function (message) {
          console.log('WebSockets: ' + message.data);
          if (isConstraintViolated) return;
          var messageData = parseMessageData(message);
          if (messageData.isComplete) {
            setLocation(xhr);
            return;
          } else if (messageData.isComplete === false) {
            isConstraintViolated = true;
            var validationMessage;

            if (messageData.duplicateUsername) {
              validationMessage = getValidationMessage({
                key: 'username',
                reason: 'alreadyExists',
                value: messageData.duplicateUsername
              }, validationMessages);
              addValidationError($form, validationMessage);
            }
            if (messageData.duplicateContact) {
              validationMessage = getValidationMessage({
                key: 'emailOrPhone',
                reason: 'alreadyExists',
                value: messageData.duplicateContact
              }, validationMessages);
              addValidationError($form, validationMessage);
            }
            if (!messageData.duplicateUsername && !messageData.duplicateContact) {
              validationMessage = getValidationMessage({
                key: 'unknown',
                reason: 'unknown'
              }, validationMessages);
              addValidationError($form, validationMessage);
            }
            enableForm($form);
            $form.find('[name=emailOrPhone]').attr('disabled', 'disabled');
          }
        }
      })
      .fail(function (xhr) {
        enableForm($form);
        $form.find('[name=emailOrPhone]').attr('disabled', 'disabled');
        handleValidationErrors(xhr, $form, validationMessages);
      });
    return false;
  });
}
