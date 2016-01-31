import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

class Form extends React.Component {
  static propTypes = {
    fields: React.PropTypes.object.isRequired,
    handleSubmit: React.PropTypes.func.isRequired,
    submitting: React.PropTypes.bool.isRequired,
  };

  render() {
    const { fields: { emailOrPhone }, handleSubmit, submitting, submitFailed } = this.props
    return(
      <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
        <div className="form-group">
          <div className="col-md-4">
            <label className="control-label sr-only">Email address or phone number</label>
            <input type="text" name="emailOrPhone" className="form-control" placeholder="Email address or phone number" disabled={submitting} {...emailOrPhone} />
          </div>
        </div>
        <div className="form-group">
          <div className="col-md-10">
            <button type="submit" className="btn btn-default" disabled={submitting}>Register</button>
          </div>
        </div>
        { submitFailed &&
          <ValidationSummary form={this.props} /> }
      </form>
    )
  }
}

const form = 'register'
const fields = ['emailOrPhone']
export default reduxForm({
  form,
  fields,
  validate,
})(Form)
