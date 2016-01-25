import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import { createAction, handleActions } from 'redux-actions'
import { SEND_WEBAPI } from '../../Shared/actions'
import { redeemMessages as messages } from './validation'
import { camelize } from 'humps'
import classNames from 'classnames'
import { formatMessage } from '../../Shared/selectors'
import Markdown from 'react-remarkable'

const CHECK_USERNAME_SENT = 'CHECK_USERNAME_SENT'
const CHECK_USERNAME_DONE = 'CHECK_USERNAME_DONE'

const actions = {
  submitCheckUsername: formInput => createAction(SEND_WEBAPI)({
    method: 'POST',
    url: '/check-username',
    formInput: formInput,
    send: () => createAction(CHECK_USERNAME_SENT)(),
    fail: (dispatch, context, response, serverErrors) => {
      const error = new TypeError('Request failed.')
      error.serverErrors = serverErrors
      //error.messages = verifyMessages
      return dispatch(createAction(CHECK_USERNAME_DONE)(error))
    },
    done: (dispatch, context, response, data) => {
      return dispatch(createAction(CHECK_USERNAME_DONE)({
        data,
        formInput: context.formInput,
        receivedAt: Date.now(),
      }))
    },
  })
}

const reducers = {
  [CHECK_USERNAME_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
      data: undefined,
    }),
  [CHECK_USERNAME_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      submitting: false,
      remoteError: action.payload,
    }),
    next: (state, action) => Object.assign({}, state, {
      submitting: false,
      data: action.payload.data,
      formInput: action.payload.formInput,
    })
  }
}

export const handlers = handleActions(reducers, { })

class CheckUsernameField extends Component {
  static propTypes = {
    field: PropTypes.object.isRequired,
  };

  submit = () => {
    const { field: { value }, dispatch } = this.props
    return new Promise((resolve, reject) => {
      return dispatch(this.props.submitCheckUsername({
        username: value,
      }))
    })
  };

  render() {
    const { field, submitting, data } = this.props
    const formGroupClassNames = classNames({
      'form-group': true,
      'has-success': data && data.isAvailable,
      'has-error': data && !data.isAvailable,
    })
    return(
      <div className={formGroupClassNames}>
        <div className="col-md-6">
          <label className="control-label sr-only">Username</label>
          <div className="input-group">
            <input type="text" className="form-control" placeholder="Choose a username" disabled={submitting} {...field} />
            <span className="input-group-btn input-group-btn-right" style={{left: '1px'}}>
              { this.renderButton() }
            </span>
          </div>
        </div>
        { this.renderHelp() }
      </div>
    )
  }

  renderButton() {
    const { submitting, data } = this.props
    return(
      <button type="button" className="btn btn-default" disabled={submitting} onClick={this.submit}>
      Check availability
      {' '}
      { data ? data.isAvailable ?
        <span className="glyphicon glyphicon-ok text-success" aria-hidden="true"></span> :
        <span className="glyphicon glyphicon-remove text-danger" aria-hidden="true"></span> :
        <span className="glyphicon glyphicon-search text-info" aria-hidden="true"></span> }
      </button>
    )
  }

  renderHelp() {
    const { submitting, data, formInput, field: { value } } = this.props
    const message = data
      ? formatMessage(messages.username[camelize(data.reasonInvalid || 'success')], formInput)
      : undefined
    return(
      <div className="col-md-12">
      { submitting ?
        <p className="help-block help-info">Checking availability...</p> :
        data ?
        <p className="help-block"><Markdown>{message}</Markdown></p> :
        <p className="help-block help-info">Use between 2 and 12 numbers, letters, hypens, underscores, and dots.</p>
      }
      </div>
    )
  }
}

function select(state, props) {
  return {
    ...state.app.ui.checkUsername,
  }
}

export default connect(select, actions)(CheckUsernameField)
