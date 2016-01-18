import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import { verifyValidate as validate } from './validation'
import { connect } from 'react-redux'
import * as actions from './actions'
import Helmet from 'react-helmet'
import { selectForm as select1 } from '../../../client/forms/reducers'
import ValidationSummary from '../../../client/forms/ValidationSummary'

class Verify extends Component {
  submit(formInput) {
    return new Promise((resolve, reject) => {
      return reject({
        code: 'The server is not ready for you yet.'
      })
      // this.props.submitVerify(this.props.dispatch, formInput)
      //   .then(() => {
      //     if (this.props.serverErrors) {
      //       return reject(this.props.serverErrors)
      //     }
      //     return resolve()
      //   })
    })
  }

  render() {
    const {
      fields: { code },
      submitVerify, handleSubmit, submitting
    } = this.props

    return(
      <div>
        <Helmet title="Verify" />
        <h2>Verify.</h2>
        <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit.bind(this))}>
          <h4>Confirm your contact information.</h4>
          <hr />
          <div className="form-group">
            <div className="col-md-3">
              <label className="control-label sr-only">Secret code</label>
              <input type="text" name="code" className="form-control" placeholder="Secret code" disabled={submitting} {...code} />
            </div>
          </div>
          <div className="form-group">
            <div className="col-md-10">
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit.bind(this))}>Verify</button>
            </div>
          </div>
          { code.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }

  static get propTypes() {
    return {
      parentUi: PropTypes.arrayOf(PropTypes.string).isRequired,
      submitting: PropTypes.bool.isRequired,
      serverErrors: PropTypes.object
    }
  }
}

function select(state) {
  return { }
}

const form = 'verify'
const fields = ['code']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(select1, actions)(Verify))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} parentUi={['register']} />
    )
  }
}
