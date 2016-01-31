import React, { Component, PropTypes } from 'react'
import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

class Form extends Component {
  static propTypes = {
    fields: PropTypes.object.isRequired,
    handleSubmit: PropTypes.func.isRequired,
    submitting: PropTypes.bool.isRequired,
  };

  render() {
    const { fields: { code }, handleSubmit, submitting, submitFailed } = this.props
    return(
      <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
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
            <button type="submit" className="btn btn-default" disabled={submitting}>Verify</button>
          </div>
        </div>
        { submitFailed &&
          <ValidationSummary form={this.props} /> }
      </form>
    )
  }
}

const form = 'verify'
const fields = ['code', 'correlationId']
export default reduxForm({
  form,
  fields,
  validate,
})(Form)
