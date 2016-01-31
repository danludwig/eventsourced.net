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
    const { fields: { code }, handleSubmit, submitting, submitFailed } = this.props
    return(
      <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
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
