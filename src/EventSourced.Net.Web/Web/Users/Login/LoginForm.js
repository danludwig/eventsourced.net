import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form';
import { connect } from 'react-redux'
import Helmet from 'react-helmet'
import * as actions from './actions'
import ValidationSummary from '../../forms/ValidationSummary'
import { messages as validationMessages } from './validation'

const fields = ['login', 'password']

const validate = values => {
  const errors = { }
  if (!values.login) errors.login = validationMessages.login.empty
  if (!values.password) errors.password = validationMessages.password.empty
  return errors;
}

class Login extends Component {
  submit(formInput) {
    return new Promise((resolve, reject) => {
      this.props.submitLogin(formInput)
        .then(() => {
          if (this.props.serverErrors) {
            return reject(this.props.serverErrors)
          }
          return resolve()
        })
    })
  }

  render() {
    const {
      fields: { login, password },
      submitLogin, handleSubmit, submitting
    } = this.props;
    return (
      <div>
        <Helmet title="Log in" />
        <h2>Log in.</h2>
        <form id="login_form" action="/api/login" method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit.bind(this))}>
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
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit.bind(this))}>Log in</button>
            </div>
          </div>
          { login.touched && password.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }

  static get propTypes() {
    return {
      fields: PropTypes.object.isRequired,
      handleSubmit: PropTypes.func.isRequired,
      submitting: PropTypes.bool.isRequired,
      serverErrors: PropTypes.object
    }
  }
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
