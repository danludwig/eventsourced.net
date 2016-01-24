import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import { validateVerify as validate } from './validation'
import { connect } from 'react-redux'
import * as actions from './actions'
import Helmet from 'react-helmet'
import { selectForm as select } from '../../Shared/selectors'
import ValidationSummary from '../../Shared/ValidationSummary'

class Verify extends Component {
  static propTypes = {
    params: PropTypes.object.isRequired,
    submitting: PropTypes.bool.isRequired,
    serverErrors: PropTypes.object
  };

  submit = (formInput, dispatch) => {
    return new Promise((resolve, reject) => {
      return dispatch(this.props.submitVerify(this.props.params.correlationId, formInput))
        .then(() => {
          if (this.props.serverErrors) {
            return reject(this.props.serverErrors)
          }
          return resolve()
        })
    })
  };

  render() {
    const { fields: { code }, handleSubmit, submitting } = this.props
    return(
      <div>
        <Helmet title="Verify" />
        <h2>Verify.</h2>
        <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit(this.submit)}>
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
              <button type="submit" className="btn btn-default" disabled={submitting} onClick={handleSubmit(this.submit)}>Verify</button>
            </div>
          </div>
          { code.touched && <ValidationSummary form={this.props} /> }
        </form>
      </div>
    )
  }
}

const form = 'verify'
const fields = ['code']
const ReduxForm = reduxForm({
  form,
  fields,
  validate
})(connect(select, actions)(Verify))

export default class Container extends Component {
  render() {
    return (
      <ReduxForm formKey={form} params={this.props.params} />
    )
  }
}
