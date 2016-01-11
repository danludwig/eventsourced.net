import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form';
import { connect } from 'react-redux'
import Helmet from 'react-helmet'
import * as actions from './actions'
import { camelize } from 'humps'

const fields = ['login', 'password']

const validationMessages = {
  login: {
    empty: 'Email or phone is required.',
    unverified: 'Invalid login or password.'
  },
  password: {
    empty: 'Password is required.'
  }
}

const validate = values => {
  const errors = { }
  if (!values.login) errors.login = validationMessages.login.empty
  if (!values.password) errors.password = validationMessages.password.empty
  return errors;
}

class Login extends Component {
  render() {
    const {
      fields: { login, password },
      submitLogin, handleSubmit, submitting
    } = this.props;
    return (
      <div>
        <Helmet title="Log in" />
        <h2>Log in.</h2>
        <form id="login_form" action="/api/login" method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(submitLogin)}>
          <h4>Use a local account to log in.</h4>
          <hr />
          <div className="form-group">
            <div className="col-md-4">
              <label className="control-label sr-only">Email address, phone number, or username</label>
              <input type="text" className="form-control" placeholder="Email address, phone number, or username" disabled={submitting} {...login} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-4">
              <label className="control-label sr-only">Password</label>
              <input type="password" className="form-control" placeholder="Password" disabled={submitting} {...password} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(submitLogin)}>Log in</button>
            </div>
          </div>
          { (login.touched && password.touched) && this.renderErrors() }
        </form>
      </div>
    )
  }

  renderErrors() {
    if (this.props.submitting) return;
    let allErrors = []
    for (var field in this.props.errors) {
      if (!this.props.errors.hasOwnProperty(field)) continue
      if (!this.props.errors[field]) continue
      allErrors.push(this.props.errors[field])
    }
    for (var field in this.props.serverErrors) {
      if (!this.props.serverErrors.hasOwnProperty(field)) continue
      if (!this.props.serverErrors[field]) continue
      for (let serverError of this.props.serverErrors[field]) {
        var reason = camelize(serverError.reason)
        if (validationMessages[field] && validationMessages[field][reason]) {
          allErrors.push(validationMessages[field][reason])
        }
        else if (serverError.message) {
          allErrors.push(serverError.message)
        }
        else {
          allErrors.push("An unknown error occurred.")
        }
      }
    }
    if (!allErrors.length) return false
    return (
      <div className="text-danger form-errors">
        <ul>
          {allErrors.map((entry, i) =>
            <li key={i}>{entry}</li>
          )}
        </ul>
      </div>
    )
  }
}

Login.propTypes = {
  fields: PropTypes.object.isRequired,
  handleSubmit: PropTypes.func.isRequired,
  submitting: PropTypes.bool.isRequired
}

function select(state) {
  return {
    submitting: state.app.ui.login.submitting || false,
    serverErrors: state.app.ui.login.serverErrors
  };
}

export default reduxForm({
  form: 'login',
  fields,
  validate
})(connect(select, actions)(Login))
